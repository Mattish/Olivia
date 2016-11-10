import cards from '../taketwo.cards';
import { Card } from '../taketwo.cards';

export default class TakeTwoListController {
    public filteredCards: Array<Card> = [];
    public addedCards: Array<Card>;
    public searchInput: string = "";
    public selectionIndex: number = 0;

    public inputChanged(event: any): void {
        this.updateFilteredCards();
    }

    public inputKeyPressed(event: KeyboardEvent): void {
        if (event.key === "Enter" && this.filteredCards.length !== 0 && this.addedCards.length <= 4) {
            this.addedCards.push(this.filteredCards[this.selectionIndex]);
            this.searchInput = "";
            this.selectionIndex = 0;
            this.updateFilteredCards();
        }
        else if (event.key === "ArrowDown") {
            this.selectionIndex = this.selectionIndex + 1 > this.filteredCards.length - 1 ? this.filteredCards.length - 1 : this.selectionIndex + 1;
        }
        else if (event.key === "ArrowUp") {
            this.selectionIndex = this.selectionIndex - 1 <= 0 ? 0 : this.selectionIndex - 1;
            var searchInputLen = this.searchInput.length;
            setTimeout(function () {
                var input: any = document.getElementById("cardSearchInput");
                input.setSelectionRange(searchInputLen, searchInputLen);
            }, 1);
        }
    }

    public selectCard(card: Card): void {
        this.addedCards.push(card);
        this.searchInput = "";
        this.selectionIndex = 0;
        this.updateFilteredCards();
    }

    updateFilteredCards(): void {
        var searchValue = this.searchInput;
        if (searchValue.length > 0) {
            this.filteredCards = cards.data.cards.filter(function (card, index, arr) {
                return card.card_name.search(searchValue) !== -1;
            })
        }
        else {
            this.filteredCards = [];
        }
    }
} 