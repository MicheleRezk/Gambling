## Gambling Game
This is Gambling Game, here you can find Rules:
### Rules
- Gambling where a random number between 0 - 9 should be generated.
- The player has a starting account of 10,000 points and can use any
  Set partial amount on the randomly generated number.
- If he is correct, he wins 9 times his stake.

###Solution
Solution consists of 3 projects:
-- Gambling.Backend
	contains all backend services for Gambling Game, using MongoDB to store players and bets, 
	-to test it, you have to first create a new player using this endpoint: /api/players
	then you can get the playerId from the response so you can use it while adding new bet using this endpoint /api/bets
-- Gambling.Backend.Tests
	contains unit test for both services and controllers.
-- Gambling.Backend.IntegrationTests
	contains the integration tests of the api services

### running integration tests
- to run the integration tests, you should have mongodb running on local on the default port 27017, if you have Docker you can run this before running the integration tests
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

### Future Features
it's nice to have other frontend project using React to build Gambling Game App and use the backend services