import { NgClass, NgTemplateOutlet } from '@angular/common';
import { Component, ContentChildren, Input, QueryList, signal } from '@angular/core';
import { TabItem } from '../../models/kanban.model';
import { TabPaneDirective } from './tab-pane.directive';

@Component({
  selector: 'app-tab',
  standalone: true,
  imports: [NgTemplateOutlet,NgClass],
  templateUrl: './tab.html',
  styleUrl: './tab.scss',
})
export class Tab {

  @ContentChildren(TabPaneDirective)
  paneItems!: QueryList<TabPaneDirective>;

  panes = signal<TabPaneDirective[]>([]);
  selectedIndex = signal(0);

  ngAfterContentInit() {
    this.panes.set(this.paneItems.toArray());

    // Listen for dynamic changes
    this.paneItems.changes.subscribe((list) => {
      this.panes.set(list.toArray());
    });
  }

  select(i: number) {
    this.selectedIndex.set(i);
  }

}
