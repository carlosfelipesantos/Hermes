import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateFreightsComponent } from './create-freights.component';

describe('CreateFreightsComponent', () => {
  let component: CreateFreightsComponent;
  let fixture: ComponentFixture<CreateFreightsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateFreightsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateFreightsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
