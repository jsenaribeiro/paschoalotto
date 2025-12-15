import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TituloListComponent } from './titulos';
import { CobrancaService } from '../../services';
import { of } from 'rxjs';
import { provideAnimations } from '@angular/platform-browser/animations';
import { describe, it, expect, beforeEach, vi } from 'vitest';
import { Titulo } from '../../models';

describe('TituloListComponent', () => {
   var component: TituloListComponent;
   var fixture: ComponentFixture<TituloListComponent>;
   var cobrancaServiceMock: any;

   const imports = [TituloListComponent];

   const providers = [
      provideAnimations(),
      { provide: CobrancaService, useValue: cobrancaServiceMock }
   ];

   const mockTitulos: Titulo[] = [
      {
         id: 1,
         numeroTitulo: '123',
         nomeDevedor: 'John Doe',
         cpfCnpj: '12345678901',
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
         total: { multa: 0, juros: 0, valor: 0 }
      }
   ];

   const getTitulos = vi.fn().mockReturnValue(of(mockTitulos));

   beforeEach(async function () {
      cobrancaServiceMock = { getTitulos };

      await TestBed.configureTestingModule({ imports, providers }).compileComponents();

      fixture = TestBed.createComponent(TituloListComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => expect(component).toBeTruthy());

   it('should load titulos on init', function () {
      component.ngOnInit();
      expect(cobrancaServiceMock.getTitulos).toHaveBeenCalled();
      expect(component.titulos()).toEqual(mockTitulos);
      expect(component.loading()).toBe(false);
   });

   it('should filter titulos', function () {
      component.titulos.set(mockTitulos);
      component.procura.set('John');
      expect(component.filtrados().length).toBe(1);

      component.procura.set('Jane');
      expect(component.filtrados().length).toBe(0);
   });
});
