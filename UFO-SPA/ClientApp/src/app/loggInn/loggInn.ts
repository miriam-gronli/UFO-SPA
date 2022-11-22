import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Observasjon } from "../Observasjon";

@Component({
  templateUrl: "loggInn.html"
})
export class LoggInn {
  skjema: FormGroup;

  validering = {
    id: [""],
    brukernavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\:. ]{2,30}")])
    ],
    passord: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\:. ]{2,30}")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router) {
    this.skjema = fb.group(this.validering);
  }

  vedSubmit() {
    this.lagreObservasjon();
  }

  lagreObservasjon() {
    const lagretObservasjon = new Observasjon();

    lagretObservasjon.navn = this.skjema.value.navn;
    lagretObservasjon.postkode = this.skjema.value.postkode;
    lagretObservasjon.beskrivelse = this.skjema.value.beskrivelse;
    lagretObservasjon.dato = this.skjema.value.dato;
    lagretObservasjon.tid = this.skjema.value.tid;

    this.http.post("api/observasjon", lagretObservasjon)
      .subscribe(retur => {
        this.router.navigate(['/liste']);
      },
        error => console.log(error)
      );
  };
}

