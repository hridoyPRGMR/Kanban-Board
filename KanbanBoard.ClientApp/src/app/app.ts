import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  isSidebarExpanded = signal(true);

  toggleSidebar(expanded: boolean) {
    this.isSidebarExpanded.set(expanded);
  }
}