import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from "react-router-dom";
import { UserContext } from './UserContext';
import "./MainStyle.css"
import Popup from './Popup'
import RightSidebar from './RightSidebar'
import UserPosts from './UserPosts'

const MainPage = () => {
    const { loggedinUser } = useContext(UserContext);
    const [posts, setPost] = useState([]);
    const [popupButton, setPopupButton] = useState(false)
    const [createPost, setCreatePost] = useState
        ({
            text: "",
            userId: loggedinUser ? loggedinUser.userId : null,
            imageURl: null,
        });

    const handleSubmit = (event) => {
        event.preventDefault();
        const formData = new FormData();



        if (loggedinUser) {
            formData.append('userId', loggedinUser.userId); // Use loggedinUser.userId here
        } else {
            alert("Please log in before posting");
            return;
        }

        if (!createPost.text && !createPost.imageURl) {
            alert("Please provide either text or an image for your post");
            return;
        }

        if (createPost.text) { 
            formData.append('text', createPost.text);
        }


        if (createPost.imageURl) {
            formData.append('image', createPost.imageURl);
        }

        fetch("twitter/posts", {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    console.log("post created");
                    getPosts();
                    setCreatePost({ text: "", userId: loggedinUser ? loggedinUser.userId : null, imageURl: null });
                } else {
                    console.error('Error creating item');
                }
            })
            .catch(error => {
                console.error('Error creating item', error);
            });
    };

    const handleChange = (event) => {
        setCreatePost({
            ...createPost,
            [event.target.name]: event.target.value
        });
    }


    const navigate = useNavigate();
    const Redirectdetail = (id) => {
        navigate('/profile/' + id)
    }




    const getPosts = async () => {
        const res = await fetch("twitter/posts")
        const body = await res.json()
        setPost(body)
    }

    const handlePostUpdate = () => {
        getPosts();
    };

    useEffect(() => {
        getPosts();
    }, [])


    return (
        
        <div className="twitter-container">
            <main className="main">

                <div>
                <div className="left-sidebar">
                    <h2>Trending</h2>
                    <ul>
                        <li>#hashtag1</li>
                        <li>#hashtag2</li>
                        <li>#hashtag3</li>
                        <li>#hashtag4</li>
                    </ul>
                    </div>
                    <button onClick={() => setPopupButton(true)} className="post-button">Make Post</button>
                </div>
                <div className="content">

                    
                    <div>
                        <h2 className="latest-tweet">Latest Tweets</h2>
                        {posts ? (
                            posts
                            .sort((a, b) => new Date(b.date) - new Date(a.date))
                            .map((post) => (
                                <UserPosts key={post.twitterPostId} post={post} onPostUpdate={handlePostUpdate} />
                        )) ): (<p>....Loading</p>) }
                    </div>

                </div>
                <RightSidebar/>
            </main>
            <Popup trigger={popupButton} setTrigger={setPopupButton}>
                <div className="input-box" style={{ margin: "auto", width: "80vw", maxWidth: "500px" }}>
                    <form className="input-box" onSubmit={handleSubmit} style={{ margin: "auto", width: "80vw", maxWidth: "500px" }}>

                        <label>Create Post:</label>
                        <input type="file" accept="image/*" name="imageURl" onChange={(event) => setCreatePost({ ...createPost, imageURl: event.target.files[0] })} />
                        <textarea name="text" value={createPost.text} onChange={handleChange} />
                        <button type="submit">Submit</button>
                    </form>
                </div>
            </Popup>
            </div>
       
    );
}

export default MainPage;

