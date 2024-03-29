Notes before starting:

Bankers & Cash
3x3, uses lines
Hit combination into freeroll minigame
Multiply a win randomly. This can very easily be used to adjust RTP:
Say the current probability of multiplying is x, with average multiplier of 3. Then the following holds:
RTPwMulti = BaseRTP _ (1 + 3 _ x)
where RTPwMulti is RTP of the game with the multiplier, but no other minigames, and BaseRTP is RTP of the game with nothing added onto it.

Crazy Diamonds
5x3, uses lines
Wild makes the entire reel wild
One symbol can be part of a combination on multiple lines
3 Scatter = free spins
During free spins, coins can spawn, if wild overrides them they give the money

Phoenix Fury
5x3, uses ways
Several ways can hit at once.
Assuming that every tile has the same probability distribution as any other tile, all ways have equal expected payouts. So it's enough to calculate the EV of one way.

Since this is my first game and I want to fully verify the result by exhaustively looping through the search space, I'll pick a small game.

To explain what I mean with an exhaustive search, I want to simulate every possible 3x3 grid, calculate it's probability and payout, and sum those up. That should return the same RTP as my "by hand" calculation, where I calculate the EV of one way and multiply it by the number of ways.
This is a way of checking my formulas in the Google sheets document, since I use two different methods (double counting) to calculate the RTP:
Google Sheet calculates the probability and payout for one way, then multiplies by 27.
C# code generates all possible boards and then evaluates each of them.

I think this wouldn't be possible on a 5x3 ways board with 8 symbols for example. Since permutations of symbols on a single reel don't matter in this game, the number of possible combinations for each reel is calculated as:
(8 choose 3 with repeats) =
((8 + 3 - 1) choose 3) =
(10 choose 3) = 120
So if I wanted to go through all of those for 5 reels, that's 120 ^ 5 = 24.883.200.000 combinations, which is too much to cycle through, considering every payout/probability computations requires a bit of time.

Short rules:
3x3 board with 8 symbols, using ways. Tiles are independent of each other. One of the symbols will be Scatter (hitting all 3 on a way gives free rerolls), another will be a 2x Wild (replaces any symbol except scatter), the others will be playing cards from 2 to 7, with each of them having their own probability and payouts. A Way only pays out if all three of the spots have the same symbol.

Symbol probabilities, payouts can be seen in the sheet:
https://docs.google.com/spreadsheets/d/1-FaA939JWSmm_OtQYgXuqNWeH4UA_FRuOuzLrKPLuQk/edit#gid=0
