import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SlideOver } from './slide-over';

describe('SlideOver', () => {
  let component: SlideOver;
  let fixture: ComponentFixture<SlideOver>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SlideOver]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SlideOver);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
