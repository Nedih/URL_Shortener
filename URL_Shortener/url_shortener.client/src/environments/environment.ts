export const environment = {
  production: false,
  apiBaseUrl: 'https://localhost:7214', //5142
  apiBaseUrl2: 'https://localhost:5142',//7214
  aboutPageText: `How to Convert an ID to a Shortened URL
This page explains how a unique numeric ID can be converted into a shortened URL using a bijection-based algorithm. The process involves mapping the numeric ID to a short string using a base-62 encoding system (with characters a-z, A-Z, and 0-9).

The algorithm works by:
1. Dividing the numeric ID by 62 and recording the remainder.
2. Mapping the remainder to a character from the defined alphabet.
3. Repeating the process with the quotient until it becomes 0.

The result is a short, unique string that can be used as the shortened URL. This system ensures that each ID maps to a unique shortened URL and vice versa, making the process both reversible and collision-free.`
};
