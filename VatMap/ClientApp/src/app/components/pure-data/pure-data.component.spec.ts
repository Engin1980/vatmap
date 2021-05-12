import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PureDataComponent } from './pure-data.component';

describe('PureDataComponent', () => {
  let component: PureDataComponent;
  let fixture: ComponentFixture<PureDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PureDataComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PureDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
