import cards from './taketwo.cards';
import { Cards, Card, Leader } from './taketwo.cards';

export default class TakeTwoController {
    public cards: Cards = cards;
    public leader: Leader = Leader.Erika;
    public leaderName: string = Leader[Leader.Erika];

    public addedCards: Array<Card> = [];
    public selectedCards: Array<Card> = [];
    public canPickLeft: boolean;
    public canPickRight: boolean;

    public mouseOverLeft(): void {
        if (this.addedCards.length >= 2) {
            this.canPickLeft = true;
        }
        this.canPickRight = false;
    }

    public mouseOverRight(): void {
        this.canPickLeft = false;
        if (this.addedCards.length >= 4) {
            this.canPickRight = true;
        }
    }

    public mouseLeaveLeft(): void {
        if (this.canPickLeft) {
            this.canPickLeft = false;
        }
    }

    public mouseLeaveRight(): void {
        if (this.canPickRight) {
            this.canPickRight = false;
        }
    }

    public mouseClick(): void {
        if (this.canPickLeft) {
            this.selectedCards.push(this.addedCards[0]);
            this.selectedCards.push(this.addedCards[1]);
        }
        else if (this.canPickRight) {
            this.selectedCards.push(this.addedCards[2]);
            this.selectedCards.push(this.addedCards[3]);
        }
        this.addedCards = [];
    }
} 