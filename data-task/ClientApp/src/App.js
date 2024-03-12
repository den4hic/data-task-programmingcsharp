import React, { useState } from 'react';
import BirthdayForm from './components/PersonForm';
import BirthdayResult from './components/PersonResult';

const App = () => {
    const [result, setResult] = useState(null);
    const [error, setError] = useState(null);

    const handleDateChange = async (personalInfo) => {
        const { firstName, lastName, email, birthdate } = personalInfo;

        console.log(personalInfo);

        try {
            const response = await fetch('person', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ firstName, lastName, email, birthdate }),
            });

            if (!response.ok) {
                if (response.status === 400) {
                    const result = await response.json();
                    setError(true);
                    setResult(result.errorMessage);
                } else {
                    throw new Error('Can not convert the data');
                }
            } else {
                const result = await response.json();
                console.log(result);
                setResult(result);
                setError(false);
            }
        } catch (error) {
            console.error(error.message);
        }
    };

    return (
        <div style={{ textAlign: 'center', padding: '20px' }}>
            <h1 style={{ color: '#333' }}>Birthday App</h1>
            <BirthdayForm onSubmit={handleDateChange} />
            {result && !error && <BirthdayResult {...result} />}
            {error && <p style={{ color: 'red' }}>{result}</p>}
        </div>
    );
};

export default App;
