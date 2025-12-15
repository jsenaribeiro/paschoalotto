import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { InputTextModule } from "primeng/inputtext";

@Component({
    selector: 'app-search-input',
    standalone: true,
    imports: [CommonModule, InputTextModule],
    templateUrl: './search-input.html'
})
export class SearchInputComponent {
    @Input() placeholder = 'Pesquisar...';
    @Output() search = new EventEmitter<string>();

    onInput(event: Event): void {
        const target = event.target as HTMLInputElement;
        this.search.emit(target.value);
    }
}