import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PureDataComponent } from './components/pure-data/pure-data.component';

const routes: Routes = [
  { path: "pure", component: PureDataComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
