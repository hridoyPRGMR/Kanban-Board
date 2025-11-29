import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from './features/layout/sidebar/sidebar';
import { Header } from './features/layout/header/header';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Sidebar, Header],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  isSidebarExpanded = signal(true);

  toggleSidebar(expanded: boolean) {
    this.isSidebarExpanded.set(expanded);
  }
}