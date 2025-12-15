import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { InputTextModule } from "primeng/inputtext";
import { TableModule } from "primeng/table";
import { ToolbarModule } from "primeng/toolbar";
import { PageHeaderComponent, StatusBadgeComponent, SearchInputComponent } from "../components";

export default [
   CommonModule,
   RouterModule,
   ButtonModule,
   CardModule,
   ToolbarModule,
   TableModule,
   FormsModule,
   InputTextModule,
   PageHeaderComponent,
   StatusBadgeComponent,
   SearchInputComponent,
];