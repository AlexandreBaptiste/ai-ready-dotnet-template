import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import type { RecipeDto } from '../../api/recipesApi';
import { deleteRecipe, getRecipes } from '../../api/recipesApi';

export default function RecipeListPage() {
  const [recipes, setRecipes] = useState<RecipeDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const load = () => {
    setLoading(true);
    getRecipes()
      .then(setRecipes)
      .catch((e: Error) => setError(e.message))
      .finally(() => setLoading(false));
  };

  useEffect(load, []);

  const handleDelete = async (id: string) => {
    if (!confirm('Delete this recipe?')) return;
    try {
      await deleteRecipe(id);
      load();
    } catch (e: unknown) {
      alert((e as Error).message);
    }
  };

  const difficultyBadge: Record<string, string> = {
    Easy: 'bg-green-100 text-green-800',
    Medium: 'bg-yellow-100 text-yellow-800',
    Hard: 'bg-red-100 text-red-800',
  };

  return (
    <div className="p-6">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Recipes</h1>
        <button
          onClick={() => navigate('/recipes/new')}
          className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-lg text-sm font-medium"
        >
          + New Recipe
        </button>
      </div>

      {loading && <p className="text-gray-500">Loading…</p>}
      {error && <p className="text-red-600">{error}</p>}

      {!loading && !error && recipes.length === 0 && (
        <p className="text-gray-500">No recipes yet. Create one!</p>
      )}

      {!loading && recipes.length > 0 && (
        <div className="overflow-x-auto">
          <table className="min-w-full bg-white rounded-xl shadow">
            <thead className="bg-gray-50 text-gray-600 text-sm uppercase">
              <tr>
                <th className="text-left px-4 py-3 w-1/3">Name</th>
                <th className="text-left px-4 py-3">Difficulty</th>
                <th className="text-left px-4 py-3">Prep (min)</th>
                <th className="text-left px-4 py-3">Category</th>
                <th className="px-4 py-3" />
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {recipes.map((r) => (
                <tr key={r.id} className="hover:bg-gray-50 transition-colors">
                  <td className="px-4 py-3 font-medium text-gray-800">{r.name}</td>
                  <td className="px-4 py-3">
                    <span
                      className={`px-2 py-0.5 rounded-full text-xs font-semibold ${difficultyBadge[r.difficulty] ?? ''}`}
                    >
                      {r.difficulty}
                    </span>
                  </td>
                  <td className="px-4 py-3 text-gray-600">{r.prepTimeMinutes}</td>
                  <td className="px-4 py-3 text-gray-600">{r.categoryId}</td>
                  <td className="px-4 py-3 flex gap-2 justify-end">
                    <Link
                      to={`/recipes/${r.id}/edit`}
                      className="text-indigo-600 hover:text-indigo-800 text-sm"
                    >
                      Edit
                    </Link>
                    <button
                      onClick={() => handleDelete(r.id)}
                      className="text-red-600 hover:text-red-800 text-sm"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
