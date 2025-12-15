import { type ComponentFixture, TestBed } from "@angular/core/testing";
import { provideAnimations } from "@angular/platform-browser/animations";
import { of } from "rxjs";
import { beforeEach, describe, expect, it, vi } from "vitest";
import type { Titulo } from "../../models";
import { CobrancaService } from "../../services";
import { TituloListComponent } from "./titulos";

describe("TituloListComponent", () => {
   var component: TituloListComponent;
   var fixture: ComponentFixture<TituloListComponent>;
   var cobrancaServiceMock: Partial<CobrancaService>;

   const imports = [TituloListComponent];

   const mockTitulos: Titulo[] = [
      {
         id: 1,
         numeroTitulo: "123",
         nomeDevedor: "John Doe",
         cpfCnpj: "12345678901",
         valorAtualizado: 100,
         status: 0,
         quantidadeParcelas: 1,
         valorOriginal: 100,
         diasEmAtraso: 10,
         multa: 10,
         jurosTotais: 10,
         dataVencimento: new Date(),
         dataEmissao: new Date(),
         parcelas: [],
         total: { multa: 0, juros: 0, valor: 0 },
      },
   ];

   beforeEach(async () => {
      const getTitulos = vi.fn().mockReturnValue(of(mockTitulos));

      cobrancaServiceMock = { getTitulos };

      const cobrancaProvider = { provide: CobrancaService, useValue: cobrancaServiceMock }

      const providers = [ provideAnimations(), cobrancaProvider ];

      await TestBed.configureTestingModule({
         imports,
         providers,
      }).compileComponents();

      fixture = TestBed.createComponent(TituloListComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it("should create", () => expect(component).toBeTruthy());

   it("should load titulos on init", () => {
      component.ngOnInit();
      expect(cobrancaServiceMock.getTitulos).toHaveBeenCalled();
      expect(component.titulos()).toEqual(mockTitulos);
      expect(component.loading()).toBe(false);
   });

   it("should filter titulos", () => {
      component.titulos.set(mockTitulos);
      component.procura.set("John");
      expect(component.filtrados().length).toBe(1);

      component.procura.set("Jane");
      expect(component.filtrados().length).toBe(0);
   });
});
