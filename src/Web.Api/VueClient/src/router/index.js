import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import ModelDetails from '@/components/ModelDetails'
import assetDetails from '@/components/AssetDetails'
import RackDiagram from '@/components/RackDiagram'
import ModelEdit from '@/components/ModelEdit'
import Models from '@/components/Models'
import assets from '@/components/Assets'
import Racks from '@/components/Racks'
import DatacenterEdit from '@/components/DatacenterEdit'
import ImportExport from '@/components/Bulk'
import Users from '@/components/Users'
import assetEdit from '@/components/AssetEdit'
import Reports from '@/components/Reports'
import App from '@/App'
import UsersCreate from '@/components/UserForm'
import Log from '@/components/Log'
import auth from '@/auth'
import DecommissionedAssets from '@/components/DecommissionedAssets'
import DecommissionedAssetDetails from '@/components/DecommissionedAssetDetails'
import ChangePlanner from '@/components/ChangePlanner'
import ChangePlanEdit from '@/components/ChangePlanEdit'
import store from '@/store/store'


Vue.use(Router)

const routes = [
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/',
        component: App,
        meta: { requiresAuth: true },
        children: [
            {
                path: '',
                name: 'dashboard',
                component: Models,
            },
            {
                path: '/models',
                name: 'models',
                component: Models,
            },
            {
                path: '/models/edit/:id',
                name: 'model-edit',
                component: ModelEdit,
                props: true,
                meta: { permission: 'model' }
            },
            {
                path: '/models/new',
                name: 'model-create',
                component: ModelEdit,
                props: true,
                meta: { permission: 'model' }
            },
            {
                path: '/models/:id',
                name: 'model-details',
                component: ModelDetails,
                props: true,
            },
            {
                path: '/assets',
                name: 'assets',
                component: assets,
            },
            {
                path: '/decommissioned-assets',
                name: 'decommissioned-assets',
                component: DecommissionedAssets,
            },
            {
                path: '/decommissioned-assets/:id',
                name: 'decommissioned-asset-details',
                component: DecommissionedAssetDetails,
                props: true,
            },
            {
                path: '/change-planner',
                name: 'change-planner',
                component: ChangePlanner,
                meta: { permission: 'asset' }
            },
            {
                path: '/change-plan/new',
                name: 'change-plan-new',
                component: ChangePlanEdit,
                meta: { permission: 'asset' }
            },
            {
                path: '/change-plan/edit/:id',
                name: 'change-plan-edit',
                component: ChangePlanEdit,
                meta: { permission: 'asset'}
            },
            {
                path: '/assets/:id',
                name: 'asset-details',
                component: assetDetails,
                props: true,
            },
            {
                path: '/assets/edit/:id',
                name: 'asset-edit',
                component: assetEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: '/assets/new',
                name: 'asset-new',
                component: assetEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: '/racks',
                name: 'racks',
                component: Racks,
            },
            {
                path: '/datacenters/edit/:id',
                name: 'datacenter-edit',
                component: DatacenterEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: '/datacenters/new',
                name: 'datacenter-create',
                component: DatacenterEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: '/importexport',
                name: 'import-export',
                component: ImportExport,
            },
            {
                path: '/racks/view',
                name: 'RackDiagram',
                component: RackDiagram,
            },
            {
                path: '/users',
                name: 'users',
                component: Users,
            },
            {
                path: '/users/new',
                name: 'users-create',
                component: UsersCreate,
                meta: { permission: 'admin' }
            },
            {
                path: '/reports',
                name: 'reports',
                component: Reports,
            },
            {
                path: '/log',
                name: 'log',
                component: Log,
                meta: { permission: 'audit' }
            }

            ]
    },
    
]

const router = new Router({
    mode: 'history',
    routes: routes
})

router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        auth.heartbeat().then(() => {
                if (!auth.loggedIn()) {
                    next({
                        path: '/login',
                        query: { redirect: to.fullPath }
                    })
                } else if (to.matched.some(record => record.meta.permission)) {
                    // implementation of nav guards for multiple permissions (ev.3)
                    if (store.getters.getPermissions.includes(to.meta.permission) || auth.isAdmin()) {
                        next()
                    } else {
                        next(false)
                    }
                } else {
                    next()
                }
            })
            .catch(() => {
                // the only reason we should get here is if the user loses internet connection or the server goes down
                // display something? a red banner at the top of the page maybe?
            });
    } else {
        next()
    }
})

export default router;