import React from 'react';
import DatePage from './components/DatePage';

const App = () => {
    const handleDateChange = async (birthdate) => {
        try {
            const response = await fetch('birthdate', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(birthdate),
            });

            if (!response.ok) {
                throw new Error('Не вдалося обробити дату народження');
            }

            const result = await response.json();
            console.log(result);
        } catch (error) {
            console.error(error.message);
        }
    };

    return (
        <div>
            <DatePage onDateChange={handleDateChange} />
        </div>
    );
};

export default App;
