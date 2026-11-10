import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// import { Login } from "./pages/login/login";

import 'iconify-icon';
import { Dashboard } from "./pages/dashboard/dashboard/dashboard";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet], //Login, Dashboard],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  // protected readonly title = signal('security-front');
}
