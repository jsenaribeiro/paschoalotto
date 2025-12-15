import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";

@Component({
	selector: "app-page-header",
	standalone: true,
	imports: [CommonModule],
	template: `
    <div class="p-4 text-center" [style]="style">
       <h1>{{ title }}</h1>
       <p *ngIf="description">{{ description }}</p>
       <ng-content></ng-content>
    </div>
  `,
})
export class PageHeaderComponent {
	@Input() title = "";
	@Input() description = "";
	@Input() style: any;
}
