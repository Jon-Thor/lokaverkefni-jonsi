import React, { createContext, useState, useEffect } from 'react';

export const UserContext = createContext({
    loggedinUser: null,
    setLoggedinUser: () => { },
    logout: () => { },
});

export const UserProvider = ({ children }) => {
    const [loggedinUser, setLoggedinUser] = useState(() => {
        const localData = localStorage.getItem('loggedinUser');
        return localData ? JSON.parse(localData) : null;
    });


    const logout = () => {
        localStorage.removeItem('loggedinUser');
        setLoggedinUser(null);
    }

    useEffect(() => {
        localStorage.setItem('loggedinUser', JSON.stringify(loggedinUser));
    }, [loggedinUser]);


    return (
        <UserContext.Provider value={{ loggedinUser, setLoggedinUser, logout }}>
            {children}
        </UserContext.Provider>
    );
};
