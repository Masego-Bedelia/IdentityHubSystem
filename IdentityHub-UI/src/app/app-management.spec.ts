import { TestBed } from '@angular/core/testing';

import { AppManagement } from './app-management';

describe('AppManagement', () => {
  let service: AppManagement;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppManagement);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
