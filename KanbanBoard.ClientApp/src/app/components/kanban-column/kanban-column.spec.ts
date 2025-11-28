import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KanabanColumn } from './kanban-column';

describe('KanabanColumn', () => {
  let component: KanabanColumn;
  let fixture: ComponentFixture<KanabanColumn>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KanabanColumn]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KanabanColumn);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
