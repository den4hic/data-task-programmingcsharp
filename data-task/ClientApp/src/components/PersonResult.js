import React from 'react';

const BirthdayResult = ({ firstName, lastName, emailAddress, birthDate, chineseSign, sunSign, isAdult, isBirthday }) => {
    return (
        <div>
            <p>First Name: {firstName}</p>
            <p>Last Name: {lastName}</p>
            <p>Email: {emailAddress}</p>
            <p>Birthdate: {birthDate}</p>
            {isAdult ? <p>You are an adult!</p> : <p>You are not an adult</p>}
            {isBirthday ? <p>Happy Birthday!</p> : <p>It is not your birthday today:(</p>}
            <p>Western Zodiac Sign: {sunSign}</p>
            <p>Chinese Zodiac Sign: {chineseSign}</p>
        </div>
    );
};

export default BirthdayResult;
