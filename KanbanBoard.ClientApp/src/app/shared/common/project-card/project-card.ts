import { DatePipe } from '@angular/common';
import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-project-card',
  imports: [DatePipe],
  templateUrl: './project-card.html',
})
export class ProjectCard {

  name = input.required<string>();
  createdAt = input<string>();

  actionClick = output<string>();

}
