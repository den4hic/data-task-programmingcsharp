// User.js
import React, { useState } from 'react';

function User({ user, editUser, deleteUser }) {
    const [isEditing, setIsEditing] = useState(false);
    const [editedUser, setEditedUser] = useState({ firstName: user.firstName, lastName: user.lastName, emailAddress: user.emailAddress, birthDate: user.birthDate });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setEditedUser(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSave = () => {
        editUser(user.id, editedUser);
        setIsEditing(false);
        setEditedUser({ firstName: user.firstName, lastName: user.lastName, emailAddress: user.emailAddress, birthDate: user.birthDate });
    };

    return (
        <tr className="user">
            <td>{isEditing ? <input type="text" name="firstName" value={editedUser.firstName} onChange={handleChange} /> :  user.firstName}</td>
            <td>{isEditing ? <input type="text" name="lastName" value={editedUser.lastName} onChange={handleChange} /> : user.lastName}</td>
            <td>{isEditing ? <input type="email" name="emailAddress" value={editedUser.emailAddress} onChange={handleChange} /> : user.emailAddress}</td>
            <td>{isEditing ? <input type="date" name="birthdate" value={editedUser.birthdate} onChange={handleChange} /> : user.birthDate}</td>
            <td>{user.isAdult ? "Yes" : "No"}</td>
            <td>{user.isBirthday ? "Yes" : "No"}</td>
            <td>{user.sunSign}</td>
            <td>{user.chineseSign}</td>
            <th>
                {isEditing ? (
                    <>
                        <button onClick={handleSave}>Save</button>
                        <button onClick={() => setIsEditing(false)}>Cancel</button>
                    </>
                ) : (
                    <button onClick={() => setIsEditing(true)}>Edit</button>
                )}
                <button onClick={() => deleteUser(user.id)}>Delete</button>
            </th>
        </tr>
    );
}

export default User;
