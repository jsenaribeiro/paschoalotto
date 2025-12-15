import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { TituloStatus } from "../../models";

@Component({
	selector: "app-status-badge",
	standalone: true,
	imports: [CommonModule],
	template: `
    <span [class]="badge.class" [title]="badge.text">
      {{ badge.text }}
    </span>
  `,
})
export class StatusBadgeComponent {
	@Input() status!: TituloStatus;

	get badge() {
		const statusArray = [
			{ class: "label label-info", text: "Em Aberto" },
			{ class: "label label-success", text: "Pago" },
			{ class: "label label-danger", text: "Cancelado" }
		];

		return statusArray[this.status] || { class: "label", text: this.status };
	}
}
