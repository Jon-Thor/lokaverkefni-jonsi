import { useState, useEffect, useContext } from 'react';
import { UserContext } from './UserContext';
import { useNavigate } from "react-router-dom";
import "./MainStyle.css"


const UserPosts = ({ post, onPostUpdate } ) => {
    const { loggedinUser } = useContext(UserContext);
    const navigate = useNavigate();
    const Redirectdetail = (id) => {
        navigate('/profile/' + id)
    }


    function handleDelete(postId) {
        const confirmDelete = window.confirm('Are you sure you want to delete this post?');

        if (confirmDelete) {
            fetch(`twitter/posts/${postId}`, {
                method: 'DELETE',
            })
                .then(response => {
                    if (response.ok) {
                        console.log("post deleted");
                        onPostUpdate();
                    } else {
                        console.error('Error deleting item');
                    }
                })
                .catch(error => {
                    console.error('Error deleting item', error);
                });
        }
    }

    function likePost(twitterPostId) {

        if (loggedinUser == null) {
            alert("Please log in before liking");
            return;
        }
        
        fetch(`twitter/${loggedinUser.userId}/like-post/${twitterPostId}`, {  // replace with your actual API URL
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Include any other headers, such as authorization headers
            },
            body: JSON.stringify({ userId: loggedinUser.userId, twitterPostId }),
        })
            .then(response => response.json())
            .then(data => {
                if (data.message) {
                    console.log(data.message);
                    onPostUpdate();
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    return <div className="tweet">
        <div style={{ display: 'flex', width: "100%", justifyContent: 'space-between' }}>
                    <div className="user-info" onClick={() => { Redirectdetail(post.userId) }}>
                <img src={post.userImage} alt={post.userName} className="user-picture" />
                <p className="user-name">{post.userName} </p>
            </div>
            {loggedinUser && post.userId === loggedinUser.userId ? (<button className="delete-button" onClick={() => handleDelete(post.twitterPostId)}>Delete</button>) : null}
            
        </div>
        <p>{post.formattedCreationDate}</p>
        <div key={post.twitterPostId} style={{ width: "100%" }}>
            <p className="tweet-text">{post.text}</p>
            {post.imageURl != null ? (
                <img src={post.imageURl} alt={post.text} className="tweet-image" />
            ) : null}
           
        </div>

        <button className="like-button" onClick={() => likePost(post.twitterPostId)}>Like {post.likesNumber.length}</button>
    </div>
}

export default UserPosts;
