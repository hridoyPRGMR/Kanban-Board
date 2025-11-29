import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';
import { Column } from '../../../models/kanban.model';

@Component({
  selector: 'app-kanban-column',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './kanban-column.html',
  styleUrl: './kanban-column.scss',
})
export class KanabanColumn {
  
  column = input.required<Column>();

  icons = {
    more: `<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 12h.01M12 12h.01M19 12h.01M6 12a1 1 0 11-2 0 1 1 0 012 0zm7 0a1 1 0 11-2 0 1 1 0 012 0zm7 0a1 1 0 11-2 0 1 1 0 012 0z"></path></svg>`,
  };
}
