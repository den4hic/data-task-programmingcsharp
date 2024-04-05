import React, { useState, useEffect } from 'react';
import User from './User';

function UserList({ users, deleteUser, editUser, fetchSortedUsers, fetchFilteredUsers }) {
    const [sortField, setSortField] = useState('');
    const [sortDirection, setSortDirection] = useState('asc');
    const [searchTerm, setSearchTerm] = useState('');
    const [searchEmail, setSearchEmail] = useState('');
    const [searchSunSign, setSearchSunSign] = useState('');
    const [searchChineseSign, setSearchChineseSign] = useState('');
    const [filterIsAdult, setFilterIsAdult] = useState('any');
    const [filterIsBirthday, setFilterIsBirthday] = useState('any'); 

    const [birthdateRange, setBirthdateRange] = useState({ start: '', end: '' });

    useEffect(() => {
        fetchSortedUsers(sortField, sortDirection);
    }, [sortField, sortDirection]);

    const handleSort = (field) => {
        if (field === sortField) {
            setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
        } else {
            setSortField(field);
            setSortDirection('asc');
        }
    };

    return (
        <div className="user-list">
            <div className="filters">
                <input
                    type="text"
                    placeholder="Search by name"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Search by email"
                    value={searchEmail}
                    onChange={(e) => setSearchEmail(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Search by sun sign"
                    value={searchSunSign}
                    onChange={(e) => setSearchSunSign(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Search by chinese sign"
                    value={searchChineseSign}
                    onChange={(e) => setSearchChineseSign(e.target.value)}
                />
                <div>
                    <label>Birthdate Range:</label>
                    <input type="date" value={birthdateRange.start} onChange={(e) => setBirthdateRange({ ...birthdateRange, start: e.target.value })} />
                    <input type="date" value={birthdateRange.end} onChange={(e) => setBirthdateRange({ ...birthdateRange, end: e.target.value })} />
                </div>
                <div>
                    <label>Adult:</label>
                    <select value={filterIsAdult} onChange={(e) => setFilterIsAdult(e.target.value)}>
                        <option value="any">Any</option>
                        <option value="true">Yes</option>
                        <option value="false">No</option>
                    </select>
                </div>
                <div>
                    <label>Birthday:</label>
                    <select value={filterIsBirthday} onChange={(e) => setFilterIsBirthday(e.target.value)}>
                        <option value="any">Any</option>
                        <option value="true">Yes</option>
                        <option value="false">No</option>
                    </select>
                </div>
                <button onClick={() => fetchFilteredUsers(searchTerm, birthdateRange, searchEmail, searchSunSign, searchChineseSign, filterIsAdult, filterIsBirthday)}>Apply Filters</button>
            </div>
            <table>
                <thead>
                    <tr>
                        <th onClick={() => handleSort('firstName')}>First Name</th>
                        <th onClick={() => handleSort('lastName')}>Last Name</th>
                        <th onClick={() => handleSort('emailAddress')}>Email</th>
                        <th onClick={() => handleSort('birthdate')}>Birthdate</th>
                        <th onClick={() => handleSort('isAdult')}>Is Adult</th>
                        <th onClick={() => handleSort('isBirthday')}>Is Birthday</th>
                        <th onClick={() => handleSort('sunSign')}>Sun sign</th>
                        <th onClick={() => handleSort('chineseSign')}>Chinese sign</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user => (
                        <User
                            key={user.id}
                            user={user}
                            editUser={editUser}
                            deleteUser={deleteUser}
                        />
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default UserList;
