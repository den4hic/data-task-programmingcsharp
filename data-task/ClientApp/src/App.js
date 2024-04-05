import React, { useState } from 'react';
import UserList from './components/UserList';
import AddUserForm from './components/AddUserForm';

function App() {
    const [users, setUsers] = useState([]);
    const [errorMessage, setErrorMessage] = useState('');
    const [isError, setIsError] = useState(false);

    const fetchSortedUsers = async (sortField, sortDirection) => {
        try {
            const response = await fetch(`/person/sorted?sortField=${sortField}&sortDirection=${sortDirection}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });
            if (!response.ok) {
                throw new Error('Failed to fetch sorted users');
            }
            
            const data = await response.json();

            console.log(data);

            setUsers(data);
        } catch (error) {
            console.error('Error fetching sorted users:', error.message);
        }
    };

    const fetchFilteredUsers = async (searchTerm, birthdateRange, searchEmail, searchSunSign, searchChineseSign, filterIsAdult, filterIsBirthday) => {
        try {
            console.log(searchTerm, birthdateRange, searchEmail, searchSunSign, searchChineseSign);
            const response = await fetch(`/person/filtered?searchTerm=${searchTerm}&startDate=${birthdateRange.start}&endDate=${birthdateRange.end}&searchEmail=${searchEmail}&searchSunSign=${searchSunSign}&searchChineseSign=${searchChineseSign}&filterIsAdult=${filterIsAdult}&filterIsBirthday=${filterIsBirthday}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });
            if (!response.ok) {
                throw new Error('Failed to fetch filtered users');
            }
            const data = await response.json();

            setUsers(data);
        } catch (error) {
            console.error('Error fetching filtered users:', error.message);
        }
    };

    const addUser = async (user) => {
        try {
            const response = await fetch('/person', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(user)
            });

            if (!response.ok) {
                const errorMessage = await response.json();
                setErrorMessage(errorMessage.errorMessage);
                setIsError(true);
                throw new Error('Failed to add user');
            }

            setIsError(false);

            const data = await response.json();
            setUsers([...users, data]);
        } catch (error) {
            console.error('Error adding user:', error.message);
        }
    };

    const deleteUser = async (userId) => {
        try {
            const response = await fetch(`/person/${userId}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                throw new Error('Failed to delete user');
            }

            const updatedUsers = users.filter(user => user.id !== userId);
            setUsers(updatedUsers);
        } catch (error) {
            console.error('Error deleting user:', error.message);
        }
    };

    const editUser = async (userId, updatedUser) => {
        try {
            const response = await fetch(`/person/${userId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ firstName: updatedUser.firstName, lastName: updatedUser.lastName, email: updatedUser.emailAddress, birthdate: updatedUser.birthDate })
            });

            if (!response.ok) {
                const errorMessage = await response.json();
                setErrorMessage(errorMessage.errorMessage);
                setIsError(true);
                throw new Error('Failed to update user');
            }

            setIsError(false);

            const data = await response.json();
            const updatedUsers = users.map(user => user.id === userId ? data : user);
            setUsers(updatedUsers);
        } catch (error) {
            console.error('Error updating user:', error.message);
        }
    };

    return (
        <div className="app">
            <h1>Users</h1>
            <div className="other-actions">
                <AddUserForm addUser={addUser} />
            </div>
            {isError && <div className="error">{errorMessage}</div>}
            <div className="container">
                <UserList
                    users={users}
                    deleteUser={deleteUser}
                    editUser={editUser}
                    fetchSortedUsers={fetchSortedUsers}
                    fetchFilteredUsers={fetchFilteredUsers}
                />
            </div>
        </div>
    );
}

export default App;
