import React, { useState, useEffect, useContext } from 'react';
import { UserContext } from './UserContext';
import { useNavigate, useParams } from "react-router-dom";
import "./MainStyle.css"
import Popup from './Popup'
import RightSidebar from './RightSidebar'
import UserPosts from './UserPosts'

const TwitterProfile = () => {
    const { id } = useParams();
    const { loggedinUser } = useContext(UserContext);
    const { logout } = useContext(UserContext)
    const navigate = useNavigate();
    const [posts, setPost] = useState([]);
    const [user, SetUser] = useState()
    const [popupButton, setPopupButton] = useState(false)
    const [upDateUser, setUpDateUser] = useState({
        userId: id,
        userName: "",
        password: "",
        email: "",
        imageUrl: null,
    });

    const getPosts = async () => {
        const res = await fetch("twitter/posts")
        const body = await res.json()
        setPost(body)
    }

    const handleSubmit = () => {
        event.preventDefault();
        const formData = new FormData();
        formData.append('userId', upDateUser.userId)
        if (upDateUser.userName.trim() !== "") {
            formData.append('userName', upDateUser.userName)
        }
        if (upDateUser.password.trim() !== "") {
            formData.append('password', upDateUser.password)
        }
        if (upDateUser.email.trim() !== "") {
            formData.append('email', upDateUser.email)
        }
        if (upDateUser.imageUrl != null) {
            formData.append('image', upDateUser.imageUrl)
        }

        
        fetch(`twitter/users/${id}`, {
            method: 'PATCH',
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error: ${response.status}`);
                }
                if (response.status === 204) {
                    console.log("User updated successfully");
                    getUser();
                } else {
                    return response.json();
                }
            })
            .then(data => {
                if (data) {
                    console.log(data);
                }
            })
            .catch(error => console.error(error));

    }


    const handleChange = (event) => {
        setUpDateUser({
            ...upDateUser,
            [event.target.name]: event.target.value
        });
    }

    const followUser = (followingId) => {


        if (loggedinUser == null) {
            alert("Please log in before following");
            return;
        }

        if (loggedinUser.userId == followingId) {
            return;
        }



        fetch(`twitter/users/${loggedinUser.userId}/follow/${followingId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ followerId: loggedinUser.userId, followingId})
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error ${response.status}`);
                } 
                getUser();
            })
            .catch(error => {
                console.error(error);
            });
    }

    function handleDelete() {
        const confirmDelete = window.confirm('Are you sure you want to delete this account?');

        if (confirmDelete) {
            alert("User deleted")

            fetch(`twitter/users/${id}`, {
                method: 'DELETE',
            })
                .then(response => {
                    if (response.ok) {
                        navigate('/');
                        logout();
                        console.log("user deleted");
                    } else {
                        console.error('Error deleting item');
                    }
                })
                .catch(error => {
                    console.error('Error deleting item', error);
                });
        }
    }

    const getUser = async () => {
        const res = await fetch("twitter/users/" + id)

        if (res.status === 404) {          
            navigate('/');
            alert("User doesn't exist")
            return;
        }
        const body = await res.json()
        SetUser(body)
    }

    const handlePostUpdate = () => {
        getPosts();
    };

    useEffect(() => {
        getUser();
        getPosts();
        window.scrollTo(0, 0)
    }, [id])


    console.log(user)

    return (
                <div className="twitter-container">
            
            <main className="main">

                <div className="left-sidebar">
                    <h2>Trending</h2>
                    <ul>
                        <li>#hashtag1</li>
                        <li>#hashtag2</li>
                        <li>#hashtag3</li>
                        <li>#hashtag4</li>
                    </ul>
                </div>



                <div className="content">
                    {user && (
                        <div className="profilePageUser" style={{ color: "white" }}>
                            <div style={{ display: 'flex', width: "100%", justifyContent: 'space-between', alignItems: 'center' }}> 
                                <img src={user.imageUrl} alt="User Avatar" className="user-picture" style={{ width: 100, height: 100 }} onClick={() => setPopupButton(true)} />
                                <button className="follow-button" onClick={() => followUser(id)} >Follow</button></div>
                            <div>
                                <p className="user-name" style={{ fontSize: 20, fontWeight: 'bold', }}>{user.userName}</p>
                                <p className="user-name" style={{ fontSize: 15, fontWeight: 'bold', }}>@{user.userName}</p>
                                <h5 className="tweet-text">
                                    Hello i am a real user do not question this.<br /><br />
                                    links to whatever @whatever
                                </h5>
                                <p>Joined 2017</p>
                                <div style={{ display: 'flex', gap: "10px" }}>
                                    <p>{user.followersIDs.length} followers</p>
                                    <p>{user.followingIDs.length} following</p>
                                </div>
                            </div>
                        </div>
                    )}

                    <div>
                        {posts ? (
                            posts
                                .filter(post => post.userId == id)
                                .sort((a, b) => new Date(b.date) - new Date(a.date))
                                .map((post) => (
                                    <UserPosts key={post.twitterPostId} post={post} onPostUpdate={handlePostUpdate} />
                                ))) : (<p>....Loading</p>)}
                    </div>


                </div>
                <RightSidebar/>
            </main>
            <Popup trigger={popupButton} setTrigger={setPopupButton}>
                <div className="input-box" style={{ margin: "auto" }}>
                    <div>
                        
                    </div>
                    <form className="input-box" onSubmit={handleSubmit}>

                        <input type="file" accept="image/*" name="imageUrl" onChange={(event) => setUpDateUser({ ...upDateUser, imageUrl: event.target.files[0]})} />
                        <label>User Name:</label>
                        <input type="text" name="userName" value={upDateUser.userName} onChange={handleChange} />

                        <label>Password:</label>
                        <input type="password" name="password" value={upDateUser.password} onChange={handleChange} />
                        <label> Email:</label>
                        <input type="email" name="email" value={upDateUser.email} onChange={handleChange} />

                        <button type="submit">Submit</button>
                        <button className="delete-button" onClick={() => handleDelete()}>Delete Item</button>
                    </form>
                </div>
            </Popup>
        </div>)
    }


export default TwitterProfile;