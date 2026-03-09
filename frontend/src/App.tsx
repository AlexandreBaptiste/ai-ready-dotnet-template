import { NavLink, Navigate, Route, Routes } from 'react-router-dom';
import RecipeListPage from './pages/recipes/RecipeListPage';
import RecipeFormPage from './pages/recipes/RecipeFormPage';
import CategoryListPage from './pages/categories/CategoryListPage';

const navLinkCls = ({ isActive }: { isActive: boolean }) =>
  `px-3 py-2 rounded-md text-sm font-medium transition-colors ${
    isActive
      ? 'bg-indigo-700 text-white'
      : 'text-indigo-100 hover:bg-indigo-700 hover:text-white'
  }`;

export default function App() {
  return (
    <div className="min-h-screen flex flex-col">
      {/* Top Navigation */}
      <nav className="bg-indigo-600 shadow">
        <div className="max-w-5xl mx-auto px-4 py-3 flex items-center gap-4">
          <span className="text-white font-bold text-lg mr-4">🥐 Pastry Recipes</span>
          <NavLink to="/recipes" className={navLinkCls}>
            Recipes
          </NavLink>
          <NavLink to="/categories" className={navLinkCls}>
            Categories
          </NavLink>
        </div>
      </nav>

      {/* Page content */}
      <main className="flex-1 max-w-5xl mx-auto w-full py-4">
        <Routes>
          <Route path="/" element={<Navigate to="/recipes" replace />} />
          <Route path="/recipes" element={<RecipeListPage />} />
          <Route path="/recipes/new" element={<RecipeFormPage />} />
          <Route path="/recipes/:id/edit" element={<RecipeFormPage />} />
          <Route path="/categories" element={<CategoryListPage />} />
        </Routes>
      </main>
    </div>
  );
}
