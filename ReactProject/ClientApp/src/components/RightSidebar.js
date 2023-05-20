import React, { useState, useEffect, useContext } from 'react';
import { UserContext } from './UserContext';
import { useNavigate } from "react-router-dom";
import "./MainStyle.css"

    const RightSidebar = () =>
    {
        const { loggedinUser } = useContext(UserContext);
        const [users, SetUser] = useState([]);
        const getUsers = async () => {
            const res = await fetch("twitter/users")
            const body = await res.json()
            SetUser(body)
        }

        const navigate = useNavigate();
        const Redirectdetail = (id) => {
            navigate('/profile/' + id)
        }

        useEffect(() => {
            getUsers();
        }, [])

        return (
            <div className="right-sidebar">
                <h2>Who to follow</h2>
                <ul>
                    {users.filter(i => loggedinUser ? i.userId !== loggedinUser.userId : true).slice(0, 5).map((user) => (
                        <li className="user-info" key={user.userId} onClick={() => { Redirectdetail(user.userId) }}>
                            <img src={user.imageUrl} alt={user.userName} className="user-picture" />
                            <p className="user-name">{user.userName} </p>
                        </li>
                    ))}
                </ul>
            </div>
            )

    }

export default RightSidebar;