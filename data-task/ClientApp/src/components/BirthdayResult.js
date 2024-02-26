import React from 'react';

const BirthdayResult = ({ age, isBirthdayToday, westernZodiacSign, chineseZodiacSign }) => {
    return (
        <div>
            <p>Your age is: {age}</p>
            {isBirthdayToday && <p>Happy Birthday!</p>}
            <p>Western Zodiac Sign: {westernZodiacSign}</p>
            <p>Chinese Zodiac Sign: {chineseZodiacSign}</p>
        </div>
    );
};

export default BirthdayResult;
