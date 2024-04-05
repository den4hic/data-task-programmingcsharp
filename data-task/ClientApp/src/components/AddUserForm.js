import React, { useState, useEffect } from 'react';

function AddUserForm({ addUser }) {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [birthdate, setBirthdate] = useState('');
    const [isFormValid, setIsFormValid] = useState(false);

    useEffect(() => {
        setIsFormValid(firstName.trim() !== '' && lastName.trim() !== '' && email.trim() !== '' && birthdate.trim() !== '');
    }, [firstName, lastName, email, birthdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!isFormValid) return;

        const newUser = {
            firstName: firstName,
            lastName: lastName,
            email: email,
            birthdate: birthdate
        };

        setFirstName('');
        setLastName('');
        setEmail('');
        setBirthdate('');

        addUser(newUser);
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>
                First Name:
                <input type="text" name="firstName" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
            </label>
            <label>
                Last Name:
                <input type="text" name="lastName" value={lastName} onChange={(e) => setLastName(e.target.value)} />
            </label>
            <label>
                Email:
                <input type="text" name="email" value={email} onChange={(e) => setEmail(e.target.value)} />
            </label>
            <label>
                Enter your birthdate:
                <input type="date" name="birthdate" value={birthdate} onChange={(e) => setBirthdate(e.target.value)} />
            </label>
            <button type="submit" disabled={!isFormValid}>Proceed</button>
        </form>
    );
}

export default AddUserForm;
