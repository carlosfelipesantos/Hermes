import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { debounceTime, distinctUntilChanged, filter } from 'rxjs/operators';

type ViaCepResponse = {
  cep?: string;
  logradouro?: string;
  bairro?: string;
  localidade?: string; // cidade
  uf?: string;
  erro?: boolean;
};

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './create-freights.component.html',
  styleUrls: ['./create-freights.component.css'],
})
export class CreateFreightsComponent {
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);

  ddds = ['11','12','13','14','15','16','17','18','19','21','31','41','51','61','71','81'];

  form = this.fb.group({
    // ORIGEM
    originCep: [''],
    originDdd: ['', Validators.required],
    originAddress: ['', Validators.required],
    originCity: ['', Validators.required],
    originUf: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(2)]],

    // DESTINO
    destCep: [''],
    destDdd: ['', Validators.required],
    destAddress: ['', Validators.required],
    destCity: ['', Validators.required],
    destUf: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(2)]],

    // CARGA
    weightKg: [null as number | null, [Validators.required, Validators.min(0.1)]],
    price: [null as number | null, [Validators.required, Validators.min(1)]],
    cargoDesc: ['', [Validators.required, Validators.minLength(10)]],
  });

  ngOnInit() {
    // origem
    this.form.controls.originCep.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      filter((v) => this.onlyDigits(v).length === 8),
    ).subscribe(() => this.fillByCep('origin'));

    // destino
    this.form.controls.destCep.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      filter((v) => this.onlyDigits(v).length === 8),
    ).subscribe(() => this.fillByCep('dest'));
  }

  submit() {
    if (this.form.invalid) return;
    console.log('FRETE (mock):', this.form.value);
  }

  // ========= CEP helpers =========

  private onlyDigits(v: string | null | undefined) {
    return (v ?? '').replace(/\D/g, '');
  }

  // opcional: mascara simples visual 00000-000
  onCepInput(ctrl: 'originCep' | 'destCep') {
    const raw = this.onlyDigits(this.form.controls[ctrl].value);
    const masked = raw.length > 5 ? `${raw.slice(0,5)}-${raw.slice(5,8)}` : raw;
    this.form.controls[ctrl].setValue(masked, { emitEvent: true });
  }

  private fillByCep(kind: 'origin' | 'dest') {
    const cepCtrl = kind === 'origin' ? this.form.controls.originCep : this.form.controls.destCep;
    const cep = this.onlyDigits(cepCtrl.value);

    this.http.get<ViaCepResponse>(`https://viacep.com.br/ws/${cep}/json/`)
      .subscribe((res) => {
        if (!res || res.erro) return;

        const address = [res.logradouro, res.bairro].filter(Boolean).join(' - ');
        if (kind === 'origin') {
          if (!this.form.controls.originAddress.value) this.form.controls.originAddress.setValue(address);
          if (!this.form.controls.originCity.value) this.form.controls.originCity.setValue(res.localidade ?? '');
          if (!this.form.controls.originUf.value) this.form.controls.originUf.setValue((res.uf ?? '').toUpperCase());
        } else {
          if (!this.form.controls.destAddress.value) this.form.controls.destAddress.setValue(address);
          if (!this.form.controls.destCity.value) this.form.controls.destCity.setValue(res.localidade ?? '');
          if (!this.form.controls.destUf.value) this.form.controls.destUf.setValue((res.uf ?? '').toUpperCase());
        }
      });
  }
}