import { useEffect, useState } from 'react';
import type { CategoryDto } from '../../api/categoriesApi';
import { createCategory, deleteCategory, getCategories, updateCategory } from '../../api/categoriesApi';

export default function CategoryListPage() {
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [newName, setNewName] = useState('');
  const [editState, setEditState] = useState<{ id: string; name: string } | null>(null);
  const [error, setError] = useState<string | null>(null);

  const load = () =>
    getCategories()
      .then(setCategories)
      .catch((e: Error) => setError(e.message));

  useEffect(() => { load(); }, []);

  const handleCreate = async () => {
    const name = newName.trim();
    if (!name) return;
    try {
      await createCategory({ name });
      setNewName('');
      load();
    } catch (e: unknown) {
      alert((e as Error).message);
    }
  };

  const handleUpdate = async () => {
    if (!editState) return;
    try {
      await updateCategory(editState.id, editState);
      setEditState(null);
      load();
    } catch (e: unknown) {
      alert((e as Error).message);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Delete this category?')) return;
    try {
      await deleteCategory(id);
      load();
    } catch (e: unknown) {
      alert((e as Error).message);
    }
  };

  const inputCls =
    'border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400';

  return (
    <div className="p-6 max-w-lg mx-auto">
      <h1 className="text-2xl font-bold text-gray-800 mb-6">Categories</h1>

      {/* Create */}
      <div className="flex gap-2 mb-6">
        <input
          className={`${inputCls} flex-1`}
          placeholder="New category name…"
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && handleCreate()}
        />
        <button
          onClick={handleCreate}
          className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-lg text-sm font-medium"
        >
          Add
        </button>
      </div>

      {error && <p className="text-red-600 mb-4">{error}</p>}

      <ul className="bg-white rounded-xl shadow divide-y divide-gray-100">
        {categories.map((c) => (
          <li key={c.id} className="flex items-center gap-3 px-4 py-3">
            {editState?.id === c.id ? (
              <>
                <input
                  className={`${inputCls} flex-1`}
                  value={editState.name}
                  onChange={(e) => setEditState({ ...editState, name: e.target.value })}
                  onKeyDown={(e) => e.key === 'Enter' && handleUpdate()}
                  autoFocus
                />
                <button
                  onClick={handleUpdate}
                  className="text-indigo-600 hover:text-indigo-800 text-sm font-medium"
                >
                  Save
                </button>
                <button
                  onClick={() => setEditState(null)}
                  className="text-gray-500 hover:text-gray-700 text-sm"
                >
                  Cancel
                </button>
              </>
            ) : (
              <>
                <span className="flex-1 text-gray-800">{c.name}</span>
                <button
                  onClick={() => setEditState({ id: c.id, name: c.name })}
                  className="text-indigo-600 hover:text-indigo-800 text-sm"
                >
                  Edit
                </button>
                <button
                  onClick={() => handleDelete(c.id)}
                  className="text-red-600 hover:text-red-800 text-sm"
                >
                  Delete
                </button>
              </>
            )}
          </li>
        ))}
        {categories.length === 0 && (
          <li className="px-4 py-3 text-gray-500 text-sm">No categories yet.</li>
        )}
      </ul>
    </div>
  );
}
