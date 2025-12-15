import { TestBed } from '@angular/core/testing';
import { CobrancaService } from './cobranca.service';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { describe, it, expect, beforeEach, afterEach } from 'vitest';
import { Titulo } from '../models';

describe('CobrancaService', () => {
  let service: CobrancaService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        CobrancaService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(CobrancaService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch titulos', () => {
    const mockTitulos: Titulo[] = [{ id: 1, numeroTitulo: '123' } as any];

    service.getTitulos().subscribe(titulos => {
      expect(titulos.length).toBe(1);
      expect(titulos).toEqual(mockTitulos);
    });

    const req = httpMock.expectOne('http://localhost:5000/api/titulos');
    expect(req.request.method).toBe('GET');
    req.flush(mockTitulos);
  });
});
