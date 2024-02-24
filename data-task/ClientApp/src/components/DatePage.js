import React, { useState } from 'react';


const DatePage = ({ onDateChange }) => {
    const [birthdate, setBirthdate] = useState('');

    const handleDateChange = (event) => {
        setBirthdate(event.target.value);
    };

    const handleSubmit = () => {
        onDateChange(birthdate);
    };

    return (
        <div>
            <label>Введіть вашу дату народження:</label>
            <input type="date" value={birthdate} onChange={handleDateChange} />
            <button onClick={handleSubmit}>Відправити</button>
        </div>
    );

};

export default DatePage;
