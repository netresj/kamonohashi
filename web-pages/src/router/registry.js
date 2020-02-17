import Index from '@/views/system-setting/registry/Index'
import Edit from '@/views/system-setting/registry/Edit'

export default [
  {
    path: '/registry',
    name: 'Registry,',
    component: Index,
    children: [
      {
        path: 'edit/:id?',
        component: Edit,
        props: true,
      },
    ],
  },
]
