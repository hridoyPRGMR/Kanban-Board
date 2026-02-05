export interface NavItem {
  path: string;
  title: string;
  icon?: string;
  exact?: boolean;
  children?: NavItem[];
}

export const NAV_ITEMS: NavItem[] = [
  {
    path: '/',
    title: 'Dashboard',
    icon: 'board',
    exact: true,
  },
  {
    path: '/projects',
    title: 'Projects',
    icon: 'project',
  },
  {
    path: '/settings',
    title: 'Settings',
    icon: 'settings',
  },
];
