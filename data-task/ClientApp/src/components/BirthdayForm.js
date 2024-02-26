import React, { useState } from 'react';

const BirthdayForm = ({ onSubmit }) => {
    const [birthdate, setBirthdate] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit({ birthdate });
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Enter your birthdate:
                <input type="date" value={birthdate} onChange={(e) => setBirthdate(e.target.value)} />
            </label>
            <button type="submit">Submit</button>
        </form>
    );
};

export default BirthdayForm;
