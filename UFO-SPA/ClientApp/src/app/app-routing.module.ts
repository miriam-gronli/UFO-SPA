import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Lagre} from './lagre/lagre';
import { Liste } from './liste/liste';
import { Endre } from './endre/endre';

const appRoots: Routes = [
  { path: 'liste', component: Liste },
  { path: 'lagre', component: Lagre },
  { path: 'endre/:id', component: Endre, },
  { path: '', redirectTo: '/loggInn', pathMatch: 'full' }
]
//"/loggInn som gjør at man kommer til loggInn.html"

@NgModule({
  imports: [
    RouterModule.forRoot(appRoots)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }