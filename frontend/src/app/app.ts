import { CommonModule } from "@angular/common";
import { Component, type OnInit } from "@angular/core";
import { RouterModule, RouterOutlet } from "@angular/router";
import { ButtonModule } from "primeng/button";
import { ToolbarModule } from "primeng/toolbar";
import { LoadingComponent } from "./shareds/loading/loading.component";

@Component({
	standalone: true,
	selector: "app-root",
	styleUrls: ["./app.css"],
	templateUrl: "./app.html",
	imports: [
		CommonModule,
		RouterOutlet,
		RouterModule,
		ButtonModule,
		ToolbarModule,
		LoadingComponent,
	],
})
export class AppComponent implements OnInit {
	public currentRoute = "";

	public ngOnInit() {
		console.log("Sistema de Cobrança iniciado");
	}

	public isActiveRoute(route: string): boolean {
		return this.currentRoute === route;
	}
}
