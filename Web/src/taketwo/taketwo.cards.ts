export enum Rarity {
    Legendary = 4,
    Gold = 3,
    Silver = 2,
    Bronze = 1
}
export class CardData {
    constructor(public cards: Card[]) { }
}
export class Cards {
    constructor(public data: CardData) { }
}

export enum Leader {
    Arisa = 1,
    Erika = 2,
    Isabelle = 3,
    Rowen = 4,
    Luna = 5,
    Urias = 6,
    Eris = 7
}

export class Card {
    public card_id: number;
    public card_name: string;
    public rarity: Rarity;
    public clan: Leader;
    public score: number;
}

var scoresByIdJson = require<string>('raw!../scores_output.json');
var jsonminify : any = require('jsonminify');
var scoresById = JSON.parse(jsonminify(scoresByIdJson));
var cardsJson = require<string>('raw!../cards.json');
var cards: Cards = JSON.parse(cardsJson)

for (var key in cards.data.cards) {

    var card = cards.data.cards[key];
    if (scoresById[card.card_id]) {
        card.score = scoresById[card.card_id];
    }
}

export default cards;