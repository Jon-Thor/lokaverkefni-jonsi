import React from 'react'
import { useEffect, useRef } from 'react';

import './PopupStyle.css'


function Popup (props)
{

    const popupRef = useRef(null);

    const handleClickOutside = (event) => {
        if (popupRef.current && !popupRef.current.contains(event.target)) {
            props.setTrigger(false);
        }
    };

    useEffect(() => {
        if (props.trigger) {
            document.addEventListener('mousedown', handleClickOutside);
        } else {
            document.removeEventListener('mousedown', handleClickOutside);
        }

        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [props.trigger]);

        return (props.trigger) ? (
            <div className="popup" >
                <div className="popup-inner" ref={popupRef}>
                    <button className="close-btn" onClick={() => props.setTrigger(false) }>close</button>
                    {props.children}
                </div>
        </div>

            ): ""
    } 


export default Popup;
