import React, { useContext, useState } from 'react';
import { useNavigate } from "react-router-dom";
import "./MainStyle.css"
import { UserContext } from './UserContext';



    const CreateUser = () =>
    {
        const [postUser, SetPostUser] = useState({
            userName: "",
            password: "",
            email: "",
            createdOnDate: new Date(),
            followers: [],
            following: [],
        })
        const [isLoginFormVisible, setIsLoginFormVisible] = useState(true);
        const [loginUserName, setLoginUserName] = useState("");
        const [loginPassword, setLoginPassword] = useState("");

        const { setLoggedinUser } = useContext(UserContext);

        const navigate = useNavigate();
        const Redirectdetail = () => {
            navigate('/')
        }

        const handleSubmit = (event) => {
            event.preventDefault();

            if (postUser.userName.trim() == "" || postUser.password.trim() == "" || postUser.email.trim() == "") {
                return alert("One or more fields are empty and must be filled")
            }

            fetch("twitter/users", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(postUser)
            })
                .then(response => {
                    if (!response.ok) {
                        if (response.status === 409) {
                            throw new Error('Username already exists');
                        }
                        throw new Error('Error creating user');
                    }
                    return response.json();
                })
                .then(newUser => {
                    setLoggedinUser(newUser);
                    Redirectdetail();
                    alert("User created and logged in");
                })
                .catch(error => {
                    console.error(error);
                    alert(error.message);
                });
        }

        const handleChange = (event) => {
            SetPostUser({
                ...postUser,
                [event.target.name]: event.target.value
            });
        }

        const handleLoginSubmit = async (event) => {
            event.preventDefault();

            try {
                const response = await fetch(`twitter/users/login`, {
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ username: loginUserName, password: loginPassword })
                });

                if (!response.ok) { // If HTTP status is not OK
                    throw new Error(`HTTP error! status: ${response.status}`); // Throw an error
                }

                const user = await response.json();
                setLoggedinUser(user)
                Redirectdetail();
                console.log(user);
                alert(`Logged in! UserId: ${user.userId}`);
            } catch (error) {
                console.error(error); // Log the error for debugging
                alert("An error occurred. Please try again later."); // Let the user know something went wrong
            }
        }



        return (
            <div className="twitter-container" style={{ maxWidth: '1200px', backgroundImage: "linear-gradient(skyblue, hotpink)", boxShadow: "0px rgba(0, 0, 0, 0)" }}>
                <main className="main" style={{ backgroundImage: "linear-gradient(skyblue, hotpink)" }}>

                    <div >


                        {isLoginFormVisible ? (

                            
                            <div className="input-box" style={{ margin: "auto" }}>
                                <button style={{ width: "200px" }} onClick={() => setIsLoginFormVisible(!isLoginFormVisible)}>
                                    {isLoginFormVisible ? "Switch to Signup" : "Switch to Login"}
                                </button>
                                    <h3>Login</h3>
                                <form className="input-box" onSubmit={handleLoginSubmit}>
                                    <label>User Name:</label>
                                    <input type="text" placeholder="Username" value={loginUserName} onChange={e => setLoginUserName(e.target.value)} required />
                                    <label >Password:</label>
                                    <input type="password" placeholder="Password" value={loginPassword} onChange={e => setLoginPassword(e.target.value)} required />
                                    <button type="submit">Log in</button>
                                </form>
                                </div>
                        ) : (
                                <div>   
                                    <div className="input-box" style={{ margin: "auto" }}>
                                        <button style={{width: "200px"}} onClick={() => setIsLoginFormVisible(!isLoginFormVisible)}>
                                            {isLoginFormVisible ? "Switch to Signup" : "Switch to Login"}
                                        </button>
                                        <h3>Sign Up</h3>
                                <form className="input-box" onSubmit={handleSubmit}>
                                    <label htmlFor="userName" >User Name:</label>
                                            <input type="text" minLength="3" maxLength="16" placeholder="Username" id="userName" name="userName" value={postUser.userName} onChange={handleChange} />

                                    <label htmlFor="password">Password:</label>
                                            <input type="password" id="password" name="password" placeholder="Password" value={postUser.password} onChange={handleChange} />

                                    <label htmlFor="email"> Email:</label>
                                            <input type="email" id="email" name="email" placeholder="Email" value={postUser.email} onChange={handleChange} />

                                    <button type="submit">Submit</button>
                                </form>
                                    </div>
                                </div>
                        )}
                    </div>

            </main>
            </div>
       
    );
    }

export default CreateUser;
