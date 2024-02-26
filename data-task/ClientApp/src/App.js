import React, { useState } from 'react';
import BirthdayForm from './components/BirthdayForm';
import BirthdayResult from './components/BirthdayResult';

const App = () => {
    const [result, setResult] = useState(null);
    const [error, setError] = useState(null);

    const handleDateChange = async (birthdate) => {
        try {
            const response = await fetch('birthday', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(birthdate.birthdate),
            });

            if (!response.ok) {
                if (response.status === 400) {
                    const result = await response.json();
                    console.log(result);
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
        <div>
            <h1>Birthday App</h1>
            <BirthdayForm onSubmit={handleDateChange} />
            {result && !error && <BirthdayResult {...result} />}
            {error && <p>{result}</p>}
        </div>
    );
};

export default App;
