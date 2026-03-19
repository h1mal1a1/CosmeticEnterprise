import { useState, useEffect } from 'react'
import { getFreeSpace } from '../utils/file/getFreeSpace';

export function useFreeSpace() {
    const [data, setData] = useState<number | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        getFreeSpace()
            .then(setData)
            .catch(err => setError(err.message))
            .finally(() => setLoading(false));
    }, []);

    return { data, loading, error };
}