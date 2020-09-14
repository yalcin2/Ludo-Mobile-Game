package mt.edu.mcast.movies;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.List;

public class MovieListAdapter extends ArrayAdapter {

    private int resource;
    private LayoutInflater inflater;
    private Context context;

    public MovieListAdapter(Context cx, int resourceId, List<Movie> objects){
        super(cx,resourceId,objects);

        this.resource = resourceId;
        this.context = cx;
        inflater = LayoutInflater.from(cx);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent){
        if(convertView ==null){
            convertView = inflater.inflate(resource,null);
        }

        TextView txtTitle = convertView.findViewById(R.id.txtTitle);
        TextView txtYear = convertView.findViewById(R.id.txtYear);
        ImageView imgMovie = convertView.findViewById(R.id.imgPost);

        final Movie objMovie = (Movie)getItem(position);
        txtTitle.setText(objMovie.getTitle());
        txtYear.setText(objMovie.getYear());
        imgMovie.setImageBitmap(objMovie.getPoster());

        convertView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String link = "http://www.imdb.com/title/" + objMovie.getId();
                Intent browserIntent = new Intent(Intent.ACTION_VIEW);
                browserIntent.setData(Uri.parse(link));
                context.startActivity(browserIntent);
            }
        });


        return convertView;
    }








}
